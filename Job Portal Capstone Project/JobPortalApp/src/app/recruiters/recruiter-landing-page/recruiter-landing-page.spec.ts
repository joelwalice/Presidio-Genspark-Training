import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecruiterLandingPage } from './recruiter-landing-page';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

// Optional: If app-recruiter-navbar is used inside, you may stub it
import { Component } from '@angular/core';
@Component({
  selector: 'app-recruiter-navbar',
  template: ''
})
class RecruiterNavbarStub {}

// describe('RecruiterLandingPage', () => {
//   let component: RecruiterLandingPage;
//   let fixture: ComponentFixture<RecruiterLandingPage>;

//   beforeEach(async () => {
//     await TestBed.configureTestingModule({
//       imports: [
//         HttpClientTestingModule,
//         RouterTestingModule
//       ],
//       declarations: [
//         RecruiterLandingPage,
//       ],
//       providers: [
//         {
//           provide: ActivatedRoute,
//           useValue: {
//             params: of({}),
//             snapshot: {
//               paramMap: {
//                 get: () => null
//               }
//             }
//           }
//         }
//       ]
//     }).compileComponents();

//     fixture = TestBed.createComponent(RecruiterLandingPage);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });
// });
