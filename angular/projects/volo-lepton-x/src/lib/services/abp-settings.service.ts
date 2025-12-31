import { Inject, Injectable, Optional } from '@angular/core';
import { filter, map, switchMap } from 'rxjs/operators';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import {
  LanguageService,
  LpxLanguage,
  UserProfileService,
} from '@volo/ngx-lepton-x.core';
import {
  ConfigStateService,
  CurrentUserDto,
  CurrentTenantDto,
  getLocaleDirection,
  LanguageInfo,
  NAVIGATE_TO_MANAGE_PROFILE,
  SessionStateService,
} from '@abp/ng.core';

import {
  NAVIGATE_TO_MY_SECURITY_LOGS,
  OPEN_MY_LINK_USERS_MODAL,
  PROFILE_PICTURE,
  ProfilePictureImage,
} from '@volo/abp.commercial.ng.ui/config';
import { UserMenuService } from '@abp/ng.theme.shared';

@Injectable({ providedIn: 'root' })
export class AbpSettingsService {
  constructor(
    private sessionService: SessionStateService,
    private configStateService: ConfigStateService,
    private languageService: LanguageService,
    private userProfileService: UserProfileService,
    @Inject(PROFILE_PICTURE)
    private profilePicture$: BehaviorSubject<ProfilePictureImage>,
    @Inject(NAVIGATE_TO_MANAGE_PROFILE) public navigateToManageProfile: any,
    @Inject(NAVIGATE_TO_MY_SECURITY_LOGS) public navigateToMySecurityLogs: any,
    @Optional() @Inject(OPEN_MY_LINK_USERS_MODAL) public openMyLinkUsersModal,
    private userMenuService: UserMenuService
  ) {}

  setUpListeners() {
    this.listenToLangChange();
    this.setLanguageOptions();
    this.setUserProfile();
    this.setProfilePicture();
    this.setUserMenuGroups();
  }

  setUserProfile() {
    const emptyStringOnNull = (value?: string) => value || '';

    const currentUser$ = this.configStateService
      .getOne$('currentUser')
      .pipe(filter<CurrentUserDto>(Boolean));

    const currentTenant$ = this.configStateService
      .getOne$('currentTenant')
      .pipe(filter<CurrentTenantDto>(Boolean));

    combineLatest([currentUser$, currentTenant$]).subscribe(
      ([currentUser, currentTenant]) => {
        this.userProfileService.patchUser({
          id: currentUser.id,
          isAuthenticated: currentUser.isAuthenticated,
          userName: currentUser.userName,
          fullName:
            emptyStringOnNull(currentUser.name) +
            ' ' +
            emptyStringOnNull(currentUser.surName),
          email: currentUser.email,
          tenant: currentTenant,
        });
      }
    );
  }

  setUserMenuGroups() {
    this.userMenuService.items$.subscribe((userMenu) => {
      const userActionGroups = userMenu.reduce(
        (acc, curr, index) => {
          const menuItem = {
            icon: curr.textTemplate?.icon,
            text: curr.textTemplate?.text,
            component: curr?.component,
            action: () => {
              curr.action();
              return of(true);
            },
            visible: curr.visible,
          };
          const setIndex = index === userMenu.length - 1 ? 1 : 0;
          acc[setIndex].push(menuItem);
          return acc;
        },
        [[], []]
      );

      this.userProfileService.patchUser({
        userActionGroups,
      });
    });
  }

  setProfilePicture() {
    this.profilePicture$
      .pipe(filter<ProfilePictureImage>(Boolean))
      .subscribe((avatar) =>
        this.userProfileService.patchUser({
          avatar: {
            type: avatar.type,
            source: avatar.source || '',
          },
        })
      );
  }

  setLanguageOptions() {
    this.sessionService
      .getLanguage$()
      .pipe(
        switchMap((currentLang) =>
          this.configStateService.getDeep$('localization.languages').pipe(
            filter<LanguageInfo[]>(Boolean),
            map((languages: LanguageInfo[]) =>
              languages.map<LpxLanguage>(({ cultureName, ...rest }) => ({
                ...rest,
                cultureName,
                selected: cultureName === currentLang,
                isRTL: getLocaleDirection(cultureName) === 'rtl',
              }))
            )
          )
        )
      )
      .subscribe((settings) => {
        this.languageService.setLanguages(settings);
      });
  }

  listenToLangChange() {
    this.languageService.selectedLanguage$
      .pipe(filter<LpxLanguage | undefined>(Boolean))
      .subscribe((lang) => {
        this.sessionService.setLanguage(lang?.cultureName || '');
      });
  }
}
