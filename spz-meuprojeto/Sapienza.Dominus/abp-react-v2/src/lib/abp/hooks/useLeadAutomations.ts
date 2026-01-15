import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadAutomationsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  }

export function useLeadAutomations(input: GetLeadAutomationsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadAutomations", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-automation", {
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

export function useAllLeadAutomations() {
  return useQuery({
    queryKey: ["leadAutomations", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-automation", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadAutomation(id: string) {
  return useQuery({
    queryKey: ["leadAutomation", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-automation/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadAutomation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-automation", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadAutomations"] });
    },
  });
}

export function useUpdateLeadAutomation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-automation/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadAutomations"] });
      queryClient.invalidateQueries({ queryKey: ["leadAutomation", data.id] });
    },
  });
}

export function useDeleteLeadAutomation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-automation/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadAutomations"] });
    },
  });
}


