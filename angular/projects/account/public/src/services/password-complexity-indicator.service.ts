import { Injectable } from "@angular/core";
import { ProgressBarStats } from '../models/password-complexity';
export interface RegexRequirementsModel {
  minLengthRegex: RegExp;
  numberRegex: RegExp;
  lowercaseRegex: RegExp;
  uppercaseRegex: RegExp;
  specialCharacterRegex: RegExp;
}

@Injectable({providedIn: 'root'})
export class PasswordComplexityIndicatorService{
  colors: string[] = ['#B0284B', '#F2A34F', '#5588A4', '#3E5CF6', '#6EBD70'];

  texts: string[] = ['Weak', 'Fair', 'Normal', 'Good', 'Strong'];

  requirements: RegexRequirementsModel = {
    minLengthRegex: /(?=.{6,})/,                                        // Default min length 6
    numberRegex: /(?=.*[0-9])/,                                         // Default isContain number
    lowercaseRegex: /(?=.*[a-z ])/,                                     // Default isContainLowercase
    uppercaseRegex: /(?=.*[A-Z])/,                                      // Default isContainUppercase
    specialCharacterRegex: /[^a-zA-Z0-9 ]+/,                             // Default isContainSpecialCharacter
  };

  validatePassword(password: string): ProgressBarStats {
    let passedCounter = 0;

    Object.values(this.requirements).forEach((reg:RegExp)=>{
      const isValid = reg.test(password);
      
      if(isValid){
        passedCounter++;
      }
    })
    return { bgColor:this.colors[passedCounter - 1], text:this.texts[passedCounter - 1], width: (100 / this.colors.length) * passedCounter };
  }
}
