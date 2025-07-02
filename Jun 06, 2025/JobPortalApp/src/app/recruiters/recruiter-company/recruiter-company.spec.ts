import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterCompany } from './recruiter-company';

describe('RecruiterCompany', () => {
  let component: RecruiterCompany;
  let fixture: ComponentFixture<RecruiterCompany>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterCompany]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterCompany);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
