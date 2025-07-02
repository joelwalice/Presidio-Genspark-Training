import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ResumeStateService {
  private _hasResume = new BehaviorSubject<boolean>(false);
  public hasResume$ = this._hasResume.asObservable();
  private _appliedJobsChanged = new BehaviorSubject<boolean>(false);
  public appliedJobsChanged$ = this._appliedJobsChanged.asObservable();

  notifyAppliedJobChange() {
    this._appliedJobsChanged.next(true);
  }
  setHasResume(value: boolean) {
    this._hasResume.next(value);
  }
}
