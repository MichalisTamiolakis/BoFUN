import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TriviaHiddenQuestionComponent } from './trivia-hidden-question.component';

describe('TriviaHiddenQuestionComponent', () => {
  let component: TriviaHiddenQuestionComponent;
  let fixture: ComponentFixture<TriviaHiddenQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TriviaHiddenQuestionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TriviaHiddenQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
