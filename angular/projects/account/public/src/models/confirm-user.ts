export interface ConfirmUserParams {
  userId: string;
  email: Partial<{
    confirmed: boolean;
    showButton: boolean;
    requireSetting: boolean;
    address: string;
  }>;
  phone: Partial<{
    confirmed: boolean;
    requireSetting: boolean;
    number?: string;
  }>;
}
