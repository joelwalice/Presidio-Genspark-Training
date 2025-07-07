import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterCompany } from './recruiter-company';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RecruiterCompany', () => {
  let component: RecruiterCompany;
  let fixture: ComponentFixture<RecruiterCompany>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterCompany, HttpClientTestingModule]
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
