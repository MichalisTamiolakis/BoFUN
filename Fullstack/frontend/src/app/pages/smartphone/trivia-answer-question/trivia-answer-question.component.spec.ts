import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TriviaAnswerQuestionComponent } from './trivia-answer-question.component';

describe('TriviaAnswerQuestionComponent', () => {
  let component: TriviaAnswerQuestionComponent;
  let fixture: ComponentFixture<TriviaAnswerQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TriviaAnswerQuestionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TriviaAnswerQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
