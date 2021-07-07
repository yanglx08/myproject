// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.PowerPlatform.Formulas.Tools.EditorState;
using Microsoft.PowerPlatform.Formulas.Tools.IR;
using Microsoft.PowerPlatform.Formulas.Tools.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.PowerPlatform.Formulas.Tools.SourceTransforms
{
    /// <summary>
    /// This class is responsible for rewriting group control definitions based on the GroupedControlsKey
    /// present in the definition of group controls.
    /// All controls present in that array should be treated as children of the group control
    /// to match the experience in studio.
    /// The implementation here is a little different from an ordinary template-based transform,
    /// since the transform deals with controls that are peers within the IR.
    /// As such, it needs to be run on the Parent of the group control, not on the group control itself.
    /// </summary>
    internal class GroupControlTransform
    {
        private static readonly string GroupControlTemplateName = "group";

        private EditorStateStore _editorStateStore;
        private ErrorContainer _errors;
        private Entropy _entropy;

        public GroupControlTransform(ErrorContainer errors, EditorStateStore editorStateStore, Entropy entropy)
        {
            _editorStateStore = editorStateStore;
            _errors = errors;
            _entropy = entropy;
        }

        public void AfterRead(BlockNode control)
        {
            var groupControls = GetGroupControlChildren(control);
            if (!groupControls.Any())
                return;

            var peerControlsDict = control.Children.ToDictionary(peer => peer.Name.Identifier, peer => peer);
            foreach (var groupControl in groupControls)
            {
                var groupControlName = groupControl.Name.Identifier;
                if (!_editorStateStore.TryGetControlState(groupControlName, out var groupControlState) || groupControlState.GroupedControlsKey.Count == 0)
                {
                    _errors.ValidationError($"Group control state is missing or empty for {groupControlName}");
                    throw new DocumentException();
                }

                _entropy.AddGroupControl(groupControlState);

                foreach (var childKey in groupControlState.GroupedControlsKey)
                {
                    if (!peerControlsDict.TryGetValue(childKey, out var newChild))
                    {
                        _errors.ValidationError($"Group control {groupControlName}'s state refers to non-existent child {childKey}");
                        throw new DocumentException();
                    }
                    groupControl.Children.Add(newChild);
                    peerControlsDict.Remove(childKey);
                }
                groupControlState.GroupedControlsKey = null;
            }
            control.Children = peerControlsDict.Values.ToList();
        }

        public void BeforeWrite(BlockNode control)
        {
            var groupControls = GetGroupControlChildren(control);
            if (!groupControls.Any())
                return;

            foreach (var groupControl in groupControls)
            {
                var groupControlName = groupControl.Name.Identifier;
                if (!_editorStateStore.TryGetControlState(groupControlName, out var groupControlState))
                {
                    // There may not be editorstate present for this. Create a fake state to use
                    groupControlState = new ControlState()
                    {
                        Name = groupControlName,
                        StyleName = "",
                        ParentIndex = int.MaxValue, // Group controls must be ordered after their children
                        IsGroupControl = true,
                        ExtensionData = ControlInfoJson.Item.CreateDefaultExtensionData()
                    };
                    _editorStateStore.TryAddControl(groupControlState);
                }

                var groupedControlNames = groupControl.Children
                    .Select(child => child.Name.Identifier)
                    .OrderBy(childName => _entropy.GetGroupControlOrder(groupControlName, childName));
                // Add the group controls to the parent's children instead
                foreach (var child in groupControl.Children)
                {
                    control.Children.Add(child);
                }
                groupControl.Children = new List<BlockNode>();
                groupControlState.GroupedControlsKey = groupedControlNames.ToList();
            }
        }

        public List<BlockNode> GetGroupControlChildren(BlockNode parent)
        {
            var gcChildren = new List<BlockNode>();
            foreach (var child in parent.Children)
            {
                if (child.Name.Kind.TypeName == GroupControlTemplateName)
                    gcChildren.Add(child);
            }
            return gcChildren;
        }
    }
}
