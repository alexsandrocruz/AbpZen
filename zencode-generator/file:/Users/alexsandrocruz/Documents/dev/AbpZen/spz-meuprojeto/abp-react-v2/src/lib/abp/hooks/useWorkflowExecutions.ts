import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetWorkflowExecutionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  workflowId?: string;
  }

export function useWorkflowExecutions(input: GetWorkflowExecutionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["workflowExecutions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workflow-execution", {
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

export function useAllWorkflowExecutions() {
  return useQuery({
    queryKey: ["workflowExecutions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workflow-execution", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useWorkflowExecution(id: string) {
  return useQuery({
    queryKey: ["workflowExecution", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/workflow-execution/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateWorkflowExecution() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/workflow-execution", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflowExecutions"] });
    },
  });
}

export function useUpdateWorkflowExecution() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/workflow-execution/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["workflowExecutions"] });
      queryClient.invalidateQueries({ queryKey: ["workflowExecution", data.id] });
    },
  });
}

export function useDeleteWorkflowExecution() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/workflow-execution/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflowExecutions"] });
    },
  });
}


