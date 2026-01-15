import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadMessageTemplatesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  }

export function useLeadMessageTemplates(input: GetLeadMessageTemplatesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadMessageTemplates", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-message-template", {
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

export function useAllLeadMessageTemplates() {
  return useQuery({
    queryKey: ["leadMessageTemplates", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-message-template", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadMessageTemplate(id: string) {
  return useQuery({
    queryKey: ["leadMessageTemplate", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-message-template/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadMessageTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-message-template", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadMessageTemplates"] });
    },
  });
}

export function useUpdateLeadMessageTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-message-template/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadMessageTemplates"] });
      queryClient.invalidateQueries({ queryKey: ["leadMessageTemplate", data.id] });
    },
  });
}

export function useDeleteLeadMessageTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-message-template/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadMessageTemplates"] });
    },
  });
}


