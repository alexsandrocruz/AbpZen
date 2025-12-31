import { LocalizationService } from "@abp/ng.core";
import { Injector } from "@angular/core";
import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

export function regexValidator(regexPattern: string, regexDescription: string, injector: Injector): ValidatorFn {
    const localizationService = injector.get(LocalizationService);
    return (control: AbstractControl): ValidationErrors | null => {
        const regexObject = new RegExp(regexPattern);
        const isPassed = regexObject.test(control.value);
        if (!regexPattern) {
            return null;
        }
        if (!isPassed && !regexDescription) {
            return { invalid: true };
        }
        if (!isPassed) {
            return {
                customMessage: {
                    customMessage: localizationService.instant({
                        key: regexDescription,
                        defaultValue: regexDescription
                    })
                },
            }
        }
        return null;
    };
}