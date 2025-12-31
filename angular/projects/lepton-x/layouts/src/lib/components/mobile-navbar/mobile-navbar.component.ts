import {
  Component,
  ContentChild,
  Injector,
  ViewEncapsulation,
  computed,
  inject,
} from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { DOCUMENT } from '@angular/common';
import { EMPTY, map } from 'rxjs';
import {
  ICON_MAP,
  LogoPanelDirective,
  MobileNavbarSettingsPanelDirective,
  MobileNavbarProfilePanelDirective,
  LpxNavbarItem,
  NavbarService,
  ToolbarService,
  UserProfile,
  UserProfileService,
} from '@volo/ngx-lepton-x.core';
import { MobileNavbarService } from './mobile-navbar.service';
import { MobileMenuTranslate } from './enums/mobile-menu-translate.enum';

@Component({
  selector: 'lpx-mobile-navbar',
  templateUrl: './mobile-navbar.component.html',
  encapsulation: ViewEncapsulation.None,
  providers: [MobileNavbarService],
})
export class MobileNavbarComponent {
  protected readonly injector = inject(Injector);
  protected readonly service = inject(MobileNavbarService);
  protected readonly toolbarService = inject(ToolbarService);
  protected readonly navbarService = inject(NavbarService);
  protected readonly profileService = inject(UserProfileService);
  protected readonly document = inject(DOCUMENT);

  @ContentChild(LogoPanelDirective) logoPanel?: LogoPanelDirective;
  @ContentChild(MobileNavbarSettingsPanelDirective)
  settingsPanel?: MobileNavbarSettingsPanelDirective;
  @ContentChild(MobileNavbarProfilePanelDirective)
  profilePanel?: MobileNavbarProfilePanelDirective;

  protected readonly currentUser = toSignal(this.profileService.user$, {
    initialValue: {} as UserProfile,
  });

  isAuthenticated = computed(() => this.currentUser().isAuthenticated);
  avatar = computed(() => this.currentUser().avatar);
  userName = computed(() => this.currentUser().userName);
  email = computed(() => this.currentUser().email);

  menuVisible = false;

  navTabs!: LpxNavbarItem[];
  selectedMenuItems!: LpxNavbarItem[];
  groupedItems$ = this.getGroupedItems();
  navbarMenuItems!: LpxNavbarItem[];
  settingsMenuItems!: LpxNavbarItem[];
  profileMenuItems?: LpxNavbarItem[];

  activeMenu = '';
  icons = ICON_MAP;
  settingsTitle = MobileMenuTranslate.SettingsTitle;
  menuTitle = MobileMenuTranslate.MenuTitle;
  toggleClass = '';

  protected getGroupedItems() {
    return this.navbarService.groupedNavbarItems$.pipe(
      map((groupItems) =>
        groupItems?.map(({ group, items }) => ({
          group,
          items: items.filter((item) => !item.showOnMobileNavbar),
        })),
      ),
    );
  }

  protected init(): void {
    this.navbarService.navbarItems$
      .pipe(takeUntilDestroyed())
      .subscribe((items) => {
        this.navTabs = items?.filter((item) => item.showOnMobileNavbar);
        this.navbarMenuItems = items?.filter(
          (item) => !this.navTabs.includes(item),
        );
      });

    this.service.settings$
      .pipe(takeUntilDestroyed())
      .subscribe((items) => (this.settingsMenuItems = items));

    this.service.userProfileActions$
      .pipe(takeUntilDestroyed())
      .subscribe((items) => (this.profileMenuItems = items));
  }

  constructor() {
    this.init();
  }

  onSubnavbarExpand(menuItem: LpxNavbarItem): void {
    if (menuItem.expanded) {
      this.navbarMenuItems
        ?.filter((item) => item !== menuItem)
        .forEach((item) => (item.expanded = false));
    }
  }

  toggleMenu(type?: string, menuItems?: LpxNavbarItem[]) {
    if (!type || !menuItems || !menuItems.length) {
      this.closeMenu();
      return;
    }

    this.toggleClass = '';
    if (type === this.activeMenu) {
      if (this.menuVisible) {
        this.closeMenu();
      }
    } else {
      this.menuVisible = true;
      this.activeMenu = type;
      this.groupedItems$ = type === 'navbar' ? this.getGroupedItems() : EMPTY;
      this.selectedMenuItems = menuItems;
    }
  }

  toggleNavbarMenu() {
    this.toggleMenu('navbar', this.navbarMenuItems);
    this.toggleClass = this.menuVisible ? 'lpx-mobile-menu-toggle-open' : '';
  }

  closeMenu() {
    this.toggleClass = '';
    this.activeMenu = '';
    this.menuVisible = false;
  }

  forceScrollToTop() {
    this.closeMenu();
    
    const scrollableContainer = this.document.querySelector(
      '.lpx-content-container',
    ) as HTMLElement;

    if (!scrollableContainer) {
      return;
    }

    if (scrollableContainer.scrollTop === 0) {
      return;
    }

    scrollableContainer.scrollTo({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }
}
