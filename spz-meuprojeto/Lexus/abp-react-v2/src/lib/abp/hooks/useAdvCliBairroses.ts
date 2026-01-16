import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliBairrosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliBairroses(input: GetAdvCliBairrosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliBairroses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-bairros", {
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

export function useAllAdvCliBairroses() {
  return useQuery({
    queryKey: ["advCliBairroses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-bairros", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliBairros(id: string) {
  return useQuery({
    queryKey: ["advCliBairros", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-bairros/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliBairros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-bairros", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliBairroses"] });
    },
  });
}

export function useUpdateAdvCliBairros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-bairros/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliBairroses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliBairros", data.id] });
    },
  });
}

export function useDeleteAdvCliBairros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-bairros/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliBairroses"] });
    },
  });
}


