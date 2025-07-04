import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterLandingPage } from './recruiter-landing-page';

describe('RecruiterLandingPage', () => {
  let component: RecruiterLandingPage;
  let fixture: ComponentFixture<RecruiterLandingPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterLandingPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterLandingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
