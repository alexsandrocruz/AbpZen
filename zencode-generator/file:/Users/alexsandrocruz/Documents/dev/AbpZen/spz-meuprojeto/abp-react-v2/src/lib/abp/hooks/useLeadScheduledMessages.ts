import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadScheduledMessagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadAutomationId?: string;
  }

export function useLeadScheduledMessages(input: GetLeadScheduledMessagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadScheduledMessages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-scheduled-message", {
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

export function useAllLeadScheduledMessages() {
  return useQuery({
    queryKey: ["leadScheduledMessages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-scheduled-message", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadScheduledMessage(id: string) {
  return useQuery({
    queryKey: ["leadScheduledMessage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-scheduled-message/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadScheduledMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-scheduled-message", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadScheduledMessages"] });
    },
  });
}

export function useUpdateLeadScheduledMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-scheduled-message/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadScheduledMessages"] });
      queryClient.invalidateQueries({ queryKey: ["leadScheduledMessage", data.id] });
    },
  });
}

export function useDeleteLeadScheduledMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-scheduled-message/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadScheduledMessages"] });
    },
  });
}


