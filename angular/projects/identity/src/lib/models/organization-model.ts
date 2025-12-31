import { BaseNode, TreeNode } from "@abp/ng.components/tree";

export interface OrganizationTreeModel{
  availableNodes: TreeNode<BaseNode>[],
  userNodes: string[],
  expandedKeys: string[],
}

