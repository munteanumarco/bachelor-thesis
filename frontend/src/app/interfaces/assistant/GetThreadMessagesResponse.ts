export interface GetThreadMessagesResponse {
  data: Array<{
    id: string;
    object: string;
    created_at: number;
    assistant_id: string | null;
    thread_id: string;
    run_id: string | null;
    role: string;
    content: Array<{
      type: string;
      text: {
        value: string;
        annotations: any[];
      };
    }>;
    file_ids: any[];
    metadata: any;
  }>;
}
