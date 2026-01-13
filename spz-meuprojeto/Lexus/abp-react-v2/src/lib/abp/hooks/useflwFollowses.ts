import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFlwFollowsesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFlwFollowses(input: GetFlwFollowsesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["flwFollowses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-follows", {
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

export function useAllFlwFollowses() {
  return useQuery({
    queryKey: ["flwFollowses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-follows", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFlwFollows(id: string) {
  return useQuery({
    queryKey: ["flwFollows", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/flw-follows/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFlwFollows() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/flw-follows", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwFollowses"] });
    },
  });
}

export function useUpdateFlwFollows() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/flw-follows/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["flwFollowses"] });
      queryClient.invalidateQueries({ queryKey: ["flwFollows", data.id] });
    },
  });
}

export function useDeleteFlwFollows() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/flw-follows/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwFollowses"] });
    },
  });
}


