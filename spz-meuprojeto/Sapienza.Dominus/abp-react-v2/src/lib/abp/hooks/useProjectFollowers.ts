import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProjectFollowersInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  projectId?: string;
  }

export function useProjectFollowers(input: GetProjectFollowersInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["projectFollowers", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-follower", {
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

export function useAllProjectFollowers() {
  return useQuery({
    queryKey: ["projectFollowers", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-follower", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProjectFollower(id: string) {
  return useQuery({
    queryKey: ["projectFollower", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/project-follower/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProjectFollower() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/project-follower", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectFollowers"] });
    },
  });
}

export function useUpdateProjectFollower() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/project-follower/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["projectFollowers"] });
      queryClient.invalidateQueries({ queryKey: ["projectFollower", data.id] });
    },
  });
}

export function useDeleteProjectFollower() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/project-follower/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectFollowers"] });
    },
  });
}


