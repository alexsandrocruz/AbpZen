import type {
  DelegateNewUserInput,
  GetUserLookupInput,
  UserDelegationDto,
  UserLookupDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityUserDelegationService {
  apiName = 'AbpAccountPublic';

  delegateNewUser = (input: DelegateNewUserInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/user-delegation/delegate-new-user',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  deleteDelegation = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/user-delegation/delete-delegation',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  getActiveDelegations = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<UserDelegationDto>>(
      {
        method: 'GET',
        url: '/api/account/user-delegation/active-delegations',
      },
      { apiName: this.apiName, ...config },
    );

  getDelegatedUsers = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<UserDelegationDto>>(
      {
        method: 'GET',
        url: '/api/account/user-delegation/delegated-users',
      },
      { apiName: this.apiName, ...config },
    );

  getMyDelegatedUsers = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<UserDelegationDto>>(
      {
        method: 'GET',
        url: '/api/account/user-delegation/my-delegated-users',
      },
      { apiName: this.apiName, ...config },
    );

  getUserLookup = (input: GetUserLookupInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<UserLookupDto>>(
      {
        method: 'GET',
        url: '/api/account/user-delegation/user-lookup',
        params: { userName: input.userName },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
