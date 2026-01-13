import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLawyersInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLawyers(input: GetLawyersInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["lawyers", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer", {
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

export function useAllLawyers() {
  return useQuery({
    queryKey: ["lawyers", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLawyer(id: string) {
  return useQuery({
    queryKey: ["lawyer", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lawyer/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLawyer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lawyer", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyers"] });
    },
  });
}

export function useUpdateLawyer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lawyer/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["lawyers"] });
      queryClient.invalidateQueries({ queryKey: ["lawyer", data.id] });
    },
  });
}

export function useDeleteLawyer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lawyer/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyers"] });
    },
  });
}


