import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetWorkspaceAccessEventsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useWorkspaceAccessEvents(input: GetWorkspaceAccessEventsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["workspaceAccessEvents", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-access-event", {
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

export function useAllWorkspaceAccessEvents() {
  return useQuery({
    queryKey: ["workspaceAccessEvents", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-access-event", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useWorkspaceAccessEvent(id: string) {
  return useQuery({
    queryKey: ["workspaceAccessEvent", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/workspace-access-event/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateWorkspaceAccessEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/workspace-access-event", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceAccessEvents"] });
    },
  });
}

export function useUpdateWorkspaceAccessEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/workspace-access-event/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["workspaceAccessEvents"] });
      queryClient.invalidateQueries({ queryKey: ["workspaceAccessEvent", data.id] });
    },
  });
}

export function useDeleteWorkspaceAccessEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/workspace-access-event/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceAccessEvents"] });
    },
  });
}


