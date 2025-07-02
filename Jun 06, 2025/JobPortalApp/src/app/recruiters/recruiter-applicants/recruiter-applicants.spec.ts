import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterApplicants } from './recruiter-applicants';

describe('RecruiterApplicants', () => {
  let component: RecruiterApplicants;
  let fixture: ComponentFixture<RecruiterApplicants>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterApplicants]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterApplicants);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
