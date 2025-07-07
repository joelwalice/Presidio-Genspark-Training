import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterProfilePage } from './recruiter-profile-page';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RecruiterService } from '../../services/recruiter/recruiter-service';

describe('RecruiterProfilePage', () => {
  let component: RecruiterProfilePage;
  let fixture: ComponentFixture<RecruiterProfilePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterProfilePage, HttpClientTestingModule],
      providers: [RecruiterService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterProfilePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
