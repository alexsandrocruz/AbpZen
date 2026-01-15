import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetTaskCommentsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  taskId?: string;
  }

export function useTaskComments(input: GetTaskCommentsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["taskComments", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/task-comment", {
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

export function useAllTaskComments() {
  return useQuery({
    queryKey: ["taskComments", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/task-comment", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useTaskComment(id: string) {
  return useQuery({
    queryKey: ["taskComment", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/task-comment/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateTaskComment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/task-comment", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["taskComments"] });
    },
  });
}

export function useUpdateTaskComment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/task-comment/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["taskComments"] });
      queryClient.invalidateQueries({ queryKey: ["taskComment", data.id] });
    },
  });
}

export function useDeleteTaskComment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/task-comment/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["taskComments"] });
    },
  });
}


