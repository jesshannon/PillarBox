import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InboxSettingsComponent } from './inbox-settings.component';

describe('InboxSettingsComponent', () => {
  let component: InboxSettingsComponent;
  let fixture: ComponentFixture<InboxSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InboxSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InboxSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
