import { MessageService } from './../services/message.service';
import { MessageDetailsModel } from './../models/MessageDetailsModel.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { trigger, style, transition, animate, keyframes, query, stagger, state } from '@angular/animations';
import { ConfirmService } from '../utils/confirm-modal-and-service';
import { InboxService } from '../services/inbox.service';

@Component({
    selector: 'app-message',
    templateUrl: './message.component.html',
    styleUrls: ['./message.component.scss'],
    animations: [
        trigger('previewWidth', [
            state('600', style({ width: '600px' })),
            state('320', style({ width: '320px' })),
            state('400', style({ width: '400px' })),
            state('99999', style({ width: '100%' })),
            transition('* <=> *', animate(200))
        ])
    ]
})
export class MessageComponent implements OnInit {

    showImages: boolean;
    width: Number
    showingOriginal: boolean;
    showingHtml: boolean;

    previewUrl: SafeResourceUrl;

    forwardRecipient: string;

    messageDetails: MessageDetailsModel;

    constructor(private route: ActivatedRoute,
        private modalService: NgbModal,
        private _sanitizer: DomSanitizer,
        private messageService: MessageService,
        private inboxService: InboxService,
        private confirmService: ConfirmService,
        private router: Router
    ) {

        this.showImages = true;
        this.showingOriginal = false;
        this.showingHtml = false;
        this.width = 99999;

        this.messageService.message$
            .subscribe(m => {
                if (m == null && this.messageDetails != null) {
                    // deleted
                    this.inboxService.refreshInbox();
                    this.router.navigate(['../'], { relativeTo: this.route });
                } else {
                    this.messageDetails = m;
                    this.updatePreview();
                }
            });
        this.route.params.subscribe(p => this.messageService.getMessage(p["id"]));

    }

    updatePreview() {

        if (this.messageDetails == null) {
            this.previewUrl = this._sanitizer.bypassSecurityTrustResourceUrl("about:blank");
        }
        else {
            var type = "Standard";
            if (this.showingHtml) {
                type = "HTML";
            } else if (this.showingOriginal) {
                type = "Original";
            }
            this.previewUrl = this._sanitizer.bypassSecurityTrustResourceUrl(`/api/ViewMessage/View?id=${this.messageDetails.id}&viewType=${type}&showImages=${this.showImages}`);
        }
    }

    ngOnInit() {
    }

    toggleViewOriginal() {
        this.showingOriginal = !this.showingOriginal;
        if (this.showingOriginal) {
            this.showingHtml = false;
        }
        this.updatePreview();
    }

    toggleViewHtml() {
        this.showingHtml = !this.showingHtml;
        if (this.showingHtml) {
            this.showingOriginal = false;
        }
        this.updatePreview();
    }

    forward() {

    }

    delete() {

        this.confirmService.confirm({ title: 'Confirm deletion', message: 'Do you really want to delete this message? This can not be undone.' }).then(
            () => {
                this.messageService.delete(this.messageDetails.id);
            },
            () => {
            });
    }

    openModal(content) {
        this.modalService.open(content).result.then((result) => {
            //this.closeResult = `Closed with: ${result}`;
            if (result == 'forward') {
                this.forward();
            }
        }, (reason) => {
            //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
    }

}
