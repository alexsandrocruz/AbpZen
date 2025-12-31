import { Avatar } from '../components/avatar';
import { LpxNavbarItem } from '../components/navbar/models';

export type UserActionGroup = LpxNavbarItem[];

export interface Tenant {
  id?: string;
  name?: string;
  isAvailable: boolean;
}

export interface UserProfile<TKey = string> {
  id?: TKey;
  isAuthenticated: boolean;
  userName: string;
  fullName: string;
  email: string;
  avatar?: Avatar;
  userActionGroups?: UserActionGroup[];
  tenant?: Tenant;
  
}
