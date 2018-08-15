import { Component, TemplateRef, ViewChild } from '@angular/core';
import { NotificationsService } from './services/notifications.service'
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap/modal/modal-ref';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';

    @ViewChild('connection')
    private connection: TemplateRef<any>;

    private connectionModalRef: NgbModalRef;

    constructor(private notificationsService: NotificationsService, private modalService: NgbModal) {

        notificationsService.connectionState.subscribe(state => {
            if (!state) {
                this.connectionModalRef = this.modalService.open(this.connection, { size: 'lg', centered: true, backdrop: 'static', keyboard: false });
            } else if (this.connectionModalRef) {
                this.connectionModalRef.close();
            }
        });

    }

    testConnection() {
    }

}
