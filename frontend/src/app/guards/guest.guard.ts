import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { map } from 'rxjs';

export const guestGuard: CanActivateFn = () => {
  const storageService: StorageService = inject(StorageService);
  const router: Router = inject(Router);

  return storageService.isLoggedIn$.pipe(
    map((isLoggedIn) => {
      if (isLoggedIn) {
        return router.parseUrl('/');
      }
      return true;
    })
  );
};
