import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetWorkspaceInvitesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useWorkspaceInvites(input: GetWorkspaceInvitesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["workspaceInvites", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-invite", {
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

export function useAllWorkspaceInvites() {
  return useQuery({
    queryKey: ["workspaceInvites", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-invite", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useWorkspaceInvite(id: string) {
  return useQuery({
    queryKey: ["workspaceInvite", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/workspace-invite/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateWorkspaceInvite() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/workspace-invite", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceInvites"] });
    },
  });
}

export function useUpdateWorkspaceInvite() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/workspace-invite/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["workspaceInvites"] });
      queryClient.invalidateQueries({ queryKey: ["workspaceInvite", data.id] });
    },
  });
}

export function useDeleteWorkspaceInvite() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/workspace-invite/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceInvites"] });
    },
  });
}


