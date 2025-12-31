import { Component, OnInit, inject, input, model, signal } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { IdentitySessionDto, IdentitySessionService } from '@volo/abp.ng.identity/proxy';

@Component({
  standalone: true,
  selector: 'abp-user-session-detail',
  templateUrl: './user-session-detail.component.html',
  imports: [ThemeSharedModule, CoreModule],
})
export class UserSessionDetailComponent implements OnInit {
  protected readonly service = inject(IdentitySessionService);

  modalOptions = { size: 'lg' };

  isModalVisible = model(false);
  sessionId = input.required<string>();
  sessionInfo = signal({} as IdentitySessionDto);

  ngOnInit(): void {
    this.service.get(this.sessionId()).subscribe(this.sessionInfo.set);
  }
}
