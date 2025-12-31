import { CoreTestingModule } from '@abp/ng.core/testing';
import { NgbTooltip, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { PersonalSettingsVerifyButtonComponent } from './personal-settings-verify-button.component';

describe('PersonalSettingsVerifyButtonComponent', () => {
  let spectator: Spectator<PersonalSettingsVerifyButtonComponent>;
  const createComponent = createComponentFactory({
    component: PersonalSettingsVerifyButtonComponent,
    imports: [CoreTestingModule.withConfig(), NgbTooltipModule],
  });

  beforeEach(
    () =>
      (spectator = createComponent({
        props: {},
      })),
  );

  it('should create component with button when nothing is passed', () => {
    testVerifyButton();
    testVerifyButtonClick();
  });

  it('should handle verified case', () => {
    testVerifiedCase();
  });

  it('should handle edited case', () => {
    testEditedCase();
  });

  function testEditedCase() {
    spectator.setInput({
      edited: true,
    });
    testIcon('times');
    testEditedLabel();
    testEditedTooltip();
  }

  function testEditedLabel() {
    const editedLabel = spectator.query('span.text-danger');
    expect(editedLabel).toExist();
    expect(editedLabel).toHaveText('NotVerified');

    const testLabel = 'test-edited-label';
    spectator.setInput({
      editedLabel: testLabel,
    });
    expect(editedLabel).toHaveText(testLabel);
  }

  function testEditedTooltip() {
    const tooltip = spectator.query(NgbTooltip);
    expect(tooltip).toExist();
    expect(tooltip.placement).toBe('top');
    expect(tooltip.ngbTooltip).toBe('FirstlySubmitToVerify');

    const testEditedTooltipText = 'test-edited-tooltip-text';
    spectator.setInput({
      editedTooltip: testEditedTooltipText,
    });

    expect(tooltip.ngbTooltip).toBe(testEditedTooltipText);
  }

  function testVerifiedCase() {
    spectator.setInput({
      verified: true,
    });
    // should hide the button when verified is set to true
    expect(spectator.query('button')).not.toExist();
    testIcon('check-square', 'text-success');
    testVerifiedLabel();
  }

  function testVerifiedLabel() {
    const verifiedLabel = spectator.query('span.text-success');
    expect(verifiedLabel).toExist();
    expect(verifiedLabel).toHaveText('Verified');

    const testLabel = 'test-verified-label';
    spectator.setInput({
      verifiedLabel: testLabel,
    });
    expect(verifiedLabel).toHaveText(testLabel);
  }

  function testVerifyButton() {
    const button = spectator.query('button');
    expect(button).toExist();
    expect(button).toHaveText('Verify');
    expect(button).toHaveClass('btn btn-warning');

    testVerifyButtonLabel(button);
    testIcon('vcard');
  }

  function testVerifyButtonClick() {
    const button = spectator.query('button');
    const btnClickSpy = jest.spyOn(spectator.component.btnClick, 'emit');

    spectator.click(button);

    expect(btnClickSpy).toHaveBeenCalledTimes(1);
  }

  function testVerifyButtonLabel(button: Element) {
    const testButtonLabel = 'test-button-label';
    spectator.setInput({
      buttonLabel: testButtonLabel,
    });

    expect(button).toHaveText(testButtonLabel);
  }

  function testIcon(type: string, testClass?: string) {
    const icon = spectator.query(`i.fa.fa-${type}`);
    expect(icon).toExist();
    if (testClass) {
      expect(icon).toHaveClass(testClass);
    }
  }
});
