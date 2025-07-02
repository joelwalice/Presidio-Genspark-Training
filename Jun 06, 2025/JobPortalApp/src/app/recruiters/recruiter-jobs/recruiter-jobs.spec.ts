import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterJobs } from './recruiter-jobs';

describe('RecruiterJobs', () => {
  let component: RecruiterJobs;
  let fixture: ComponentFixture<RecruiterJobs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterJobs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterJobs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
