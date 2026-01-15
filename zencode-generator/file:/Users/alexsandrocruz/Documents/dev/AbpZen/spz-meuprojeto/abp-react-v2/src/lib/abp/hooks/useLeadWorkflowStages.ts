import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadWorkflowStagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  }

export function useLeadWorkflowStages(input: GetLeadWorkflowStagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadWorkflowStages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-workflow-stage", {
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

export function useAllLeadWorkflowStages() {
  return useQuery({
    queryKey: ["leadWorkflowStages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-workflow-stage", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadWorkflowStage(id: string) {
  return useQuery({
    queryKey: ["leadWorkflowStage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-workflow-stage/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadWorkflowStage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-workflow-stage", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflowStages"] });
    },
  });
}

export function useUpdateLeadWorkflowStage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-workflow-stage/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflowStages"] });
      queryClient.invalidateQueries({ queryKey: ["leadWorkflowStage", data.id] });
    },
  });
}

export function useDeleteLeadWorkflowStage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-workflow-stage/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadWorkflowStages"] });
    },
  });
}


