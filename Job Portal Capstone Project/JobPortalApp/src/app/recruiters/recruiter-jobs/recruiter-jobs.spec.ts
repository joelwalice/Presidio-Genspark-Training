import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterJobs } from './recruiter-jobs';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RecruiterJobs', () => {
  let component: RecruiterJobs;
  let fixture: ComponentFixture<RecruiterJobs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterJobs, HttpClientTestingModule]
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
