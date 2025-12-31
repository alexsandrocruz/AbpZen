import { InjectionToken } from "@angular/core";
import { UppyOptions } from "@uppy/core";

export const UPPY_OPTIONS = new InjectionToken<UppyOptions<any>>('UPPY_OPTIONS');
