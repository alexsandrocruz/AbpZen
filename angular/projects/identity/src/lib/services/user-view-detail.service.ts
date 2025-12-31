import { Injectable, inject } from '@angular/core';
import { Observable, combineLatest, map } from 'rxjs';
import { BaseNode, TreeAdapter, TreeNode } from '@abp/ng.components/tree';
import { IdentityUserDto, IdentityUserService } from '@volo/abp.ng.identity/proxy';
import { OrganizationTreeModel } from '../models';

@Injectable()
export class UserViewDetailService {
  protected readonly identityService = inject(IdentityUserService);

  getUserById(id: string): Observable<IdentityUserDto> {
    if (!id) {
      return;
    }

    return this.identityService.findById(id);
  }

  getOrganizationUnits(id: string): Observable<OrganizationTreeModel> {
    const userUnits$ = this.identityService.getOrganizationUnits(id);
    const availableUnits$ = this.identityService.getAvailableOrganizationUnits();

    return combineLatest([userUnits$, availableUnits$]).pipe(
      map(([userNodes, availableNodes]) => {
        const treeAdapter = new TreeAdapter(availableNodes.items as BaseNode[]);

        const organizationTree = {
          availableNodes: treeAdapter.getTree(),
          expandedKeys: availableNodes.items.map(item => item.id),
          userNodes: userNodes.map(unit => unit.id),
        };

        this.makeNodesReadOnly(organizationTree.availableNodes);

        return organizationTree;
      }),
    );
  }

  makeNodesReadOnly(nodes: TreeNode<BaseNode>[]): void {
    for (const node of nodes) {
      node.disableCheckbox = true;

      if (!node.isLeaf) {
        this.makeNodesReadOnly(node.children);
      }
    }
  }
}
