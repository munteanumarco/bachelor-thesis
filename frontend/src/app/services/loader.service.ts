import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private loading: boolean = false;
  private manualLoading: boolean = false;

  constructor() {}

  setLoading(loading: boolean) {
    this.loading = loading;
  }

  setManualLoading(loading: boolean) {
    this.manualLoading = loading;
  }

  getLoading(): boolean {
    return this.loading || this.manualLoading;
  }
}
