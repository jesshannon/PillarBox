
// More info: http://frhagn.github.io/Typewriter/




export class PaginatedListModel<T> {
    
    // PAGEINDEX
    public pageIndex: number = 0;
    // TOTALPAGES
    public totalPages: number = 0;
    // HASPREVIOUSPAGE
    public hasPreviousPage: boolean = false;
    // HASNEXTPAGE
    public hasNextPage: boolean = false;
    // ITEMS
    public items: T[] = [];
}
