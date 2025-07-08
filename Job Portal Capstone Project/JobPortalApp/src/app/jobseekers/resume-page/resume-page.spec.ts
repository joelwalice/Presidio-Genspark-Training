import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResumePage } from './resume-page';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('JobSeeker Resume Page', () => {
  let component: ResumePage;
  let fixture: ComponentFixture<ResumePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResumePage, HttpClientTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResumePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
