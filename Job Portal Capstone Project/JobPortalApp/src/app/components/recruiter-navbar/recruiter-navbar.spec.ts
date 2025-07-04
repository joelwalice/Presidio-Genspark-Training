import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterNavbar } from './recruiter-navbar';

describe('RecruiterNavbar', () => {
  let component: RecruiterNavbar;
  let fixture: ComponentFixture<RecruiterNavbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterNavbar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterNavbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
