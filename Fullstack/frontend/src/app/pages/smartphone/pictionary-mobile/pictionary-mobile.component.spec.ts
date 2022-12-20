import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PictionaryMobileComponent } from './pictionary-mobile.component';

describe('PictionaryMobileComponent', () => {
  let component: PictionaryMobileComponent;
  let fixture: ComponentFixture<PictionaryMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PictionaryMobileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PictionaryMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
