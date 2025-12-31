import { Injectable } from '@angular/core';
import { AbstractNavTreeService, ABP } from '@abp/ng.core';

@Injectable()
export class ManageProfileTabsService extends AbstractNavTreeService<Omit<ABP.Tab, 'parentName'>> {}
