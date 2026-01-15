import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetMessageAttachmentsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  chatMessageId?: string;
  }

export function useMessageAttachments(input: GetMessageAttachmentsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["messageAttachments", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/message-attachment", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllMessageAttachments() {
  return useQuery({
    queryKey: ["messageAttachments", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/message-attachment", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useMessageAttachment(id: string) {
  return useQuery({
    queryKey: ["messageAttachment", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/message-attachment/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateMessageAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/message-attachment", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messageAttachments"] });
    },
  });
}

export function useUpdateMessageAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/message-attachment/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["messageAttachments"] });
      queryClient.invalidateQueries({ queryKey: ["messageAttachment", data.id] });
    },
  });
}

export function useDeleteMessageAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/message-attachment/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messageAttachments"] });
    },
  });
}


