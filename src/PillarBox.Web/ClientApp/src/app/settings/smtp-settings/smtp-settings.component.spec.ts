import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SmtpSettingsComponent } from './smtp-settings.component';

describe('SmtpSettingsComponent', () => {
  let component: SmtpSettingsComponent;
  let fixture: ComponentFixture<SmtpSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SmtpSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SmtpSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
