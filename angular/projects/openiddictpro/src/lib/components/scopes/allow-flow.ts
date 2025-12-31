import { Applications } from '@volo/abp.ng.openiddictpro/proxy';

export const allowFlow = ({
  allowImplicitFlow,
  allowAuthorizationCodeFlow,
  allowHybridFlow,
}: Applications.Dtos.ApplicationDto): boolean => {
  return allowAuthorizationCodeFlow || allowImplicitFlow || allowHybridFlow;
};
