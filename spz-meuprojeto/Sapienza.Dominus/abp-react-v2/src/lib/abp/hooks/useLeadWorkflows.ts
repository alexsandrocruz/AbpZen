import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadWorkflowsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLeadWorkflows(input: GetLeadWorkflowsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadWorkflows", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-workflow", {
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

export function useAllLeadWorkflows() {
  return useQuery({
    queryKey: ["leadWorkflows", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-workflow", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadWorkflow(id: string) {
  return useQuery({
    queryKey: ["leadWorkflow", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-workflow/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-workflow", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflows"] });
    },
  });
}

export function useUpdateLeadWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-workflow/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflows"] });
      queryClient.invalidateQueries({ queryKey: ["leadWorkflow", data.id] });
    },
  });
}

export function useDeleteLeadWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-workflow/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflows"] });
    },
  });
}


