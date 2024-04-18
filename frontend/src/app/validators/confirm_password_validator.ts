import { AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';

export function confirmPasswordValidator(
  confirmPasswordControlName: string
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.value;
    const confirmPassword = control.parent?.get(
      confirmPasswordControlName
    )?.value;

    if (password !== confirmPassword) {
      return { confirmPassword: true };
    }

    return null;
  };
}
