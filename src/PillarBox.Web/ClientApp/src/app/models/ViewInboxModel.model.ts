
// More info: http://frhagn.github.io/Typewriter/



import { MessageSummaryModel } from './MessageSummaryModel.model';
import { PaginatedListModel } from './PaginatedListModel.model';

export class ViewInboxModel {
    
    // ID
    public id: string = null;
    // PARENTPATH
    public parentPath: string = null;
    // NAME
    public name: string = null;
    // MESSAGES
    public messages: PaginatedListModel<MessageSummaryModel> = null;
}
